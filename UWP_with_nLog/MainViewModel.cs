using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_with_nLog
{
    public class MainViewModel
    {
        private readonly ILogger _logger;
        public MainViewModel(ILogger<MainViewModel> logger)
        {
            _logger = logger;

            _logger.LogDebug("I'm not bad :( I'm not gonna crash our app in InitializeComponent call");
        }
    }
}
