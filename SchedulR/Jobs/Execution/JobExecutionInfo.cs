using Cronos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SchedulR.Jobs.Execution;

public record JobExecutionInfo(ILogger Logger, IConfiguration Configuration);