using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using N5Challenge.Api.Core.Models;
using N5Challenge.Api.Core.Persistence;
using N5Challenge.Api.Services.PermissionsService;
using N5Challenge.Api.Services.PermissionsTypeService;
using Nest;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace N5Challenge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogger<PermissionsController> _logger;
        private readonly IElasticClient _elasticClient;

        private readonly string bootstrapServers = "localhost:9092";
        private readonly string topic = "n5-challenge";

        public PermissionsController(
            IPermissionService permissionService,
            ILogger<PermissionsController> logger,
            IElasticClient elasticClient
            )
        {
            _permissionService = permissionService;
            _logger = logger;
            _elasticClient = elasticClient;
        }

        /// <summary>
        /// Request permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RequestPermision")]
        public async Task<IActionResult> CreatePermission(PermissionsRequestDto permission)
        {
            _logger.LogInformation("Request permission");
            
            // Send message to kafka
            var message = new OperationMessage
            {
                Id = Guid.NewGuid(),
                OperationName = "request"
            };
            string messageToSend = JsonSerializer.Serialize(message);
            await SendOrderRequest(topic, messageToSend);

            var isPermisionCreated = await _permissionService.CreatePermission(permission);
            if (isPermisionCreated)
            {
                // Index permission to elasticsearch
                await _elasticClient.IndexDocumentAsync(permission);

                _logger.LogInformation("Permission Request - ", DateTime.UtcNow);

                return Ok(isPermisionCreated);
            } else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Modify permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("ModifyPermision")]
        public async Task<IActionResult> UpdatePermission(int permissionId, PermissionsRequestDto permissionModifyRequest)
        {
            _logger.LogInformation("Modify permission");

            // Send message to kafka
            var message = new OperationMessage
            {
                Id = Guid.NewGuid(),
                OperationName = "modify"
            };
            string messageToSend = JsonSerializer.Serialize(message);
            await SendOrderRequest(topic, messageToSend);

            var isPermisionUpdated = await _permissionService.UpdatePermission(permissionId, permissionModifyRequest);
            if (isPermisionUpdated)
            {
                var permissionUpdated = await _permissionService.GetPermissionById(permissionId);

                // Update permission 
                await _elasticClient.UpdateAsync<Permissions>(permissionUpdated, p => p.Doc(permissionUpdated));

                _logger.LogInformation("Permission Request - ", DateTime.UtcNow);

                return Ok(isPermisionUpdated);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get Permissions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPermisions")]
        public async Task<IActionResult> GetPermissions()
        {
            _logger.LogInformation("Get permissions list");

            // Send message to kafka
            var message = new OperationMessage
            {
                Id = Guid.NewGuid(),
                OperationName = "get"
            };
            string messageToSend = JsonSerializer.Serialize(message);
            await SendOrderRequest(topic, messageToSend);

            var permisionList = await _permissionService.GetAllPermissions();
            if (permisionList == null)
            {
                return NotFound();
            }
            else
            {
                // Index permission to elasticsearch
                //await _elasticClient.IndexDocumentAsync(permission);

                _logger.LogInformation("Permission list - ", DateTime.UtcNow);
                return Ok(permisionList);
            }
            
            //var result = await _elasticClient.SearchAsync<Permissions>(
                //s => s.Query().Size(5000));

            //_logger.LogInformation("ProductsController Get - ", DateTime.UtcNow);
            //return Ok(result.Documents.ToList());
        }

        /// <summary>
        /// Send message to kafka
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task<bool> SendOrderRequest
        (string topic, string message)
        {
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName()
            };

            try
            {
                using (var producer = new ProducerBuilder
                <Null, string>(config).Build())
                {
                    var result = await producer.ProduceAsync
                    (topic, new Message<Null, string>
                    {
                        Value = message
                    });

                    _logger.LogInformation($"Delivery kafka: { result.Timestamp.UtcDateTime} ");
                return await Task.FromResult(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error kafka: {ex.Message}");
            }

            return await Task.FromResult(false);
        }
    }
}
