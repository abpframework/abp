using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Volo.ClientSimulation.Web.Pages
{
    public class SimulationAreaModel : PageModel
    {
        public Simulation Simulation { get; }

        public SimulationAreaModel(Simulation simulation)
        {
            Simulation = simulation;
        }

        public Task OnGetAsync()
        {
            return Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostStartAsync()
        {
            Simulation.Start();
            return new NoContentResult();
        }

        public async Task<IActionResult> OnPostStopAsync()
        {
            Simulation.Stop();
            return new NoContentResult();
        }
    }
}