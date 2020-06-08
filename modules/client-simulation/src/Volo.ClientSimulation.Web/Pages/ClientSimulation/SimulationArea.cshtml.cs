using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.ClientSimulation.Snapshot;

namespace Volo.ClientSimulation.Pages.ClientSimulation
{
    public class SimulationAreaModel : PageModel
    {
        public SimulationSnapshot Snapshot { get; private set; }

        protected Simulation Simulation { get; }

        public SimulationAreaModel(Simulation simulation)
        {
            Simulation = simulation;
        }

        public virtual Task<IActionResult> OnGetAsync()
        {
            Snapshot = Simulation.CreateSnapshot();
            return Task.FromResult<IActionResult>(Page());
        }

        public virtual async Task<IActionResult> OnPostStartAsync()
        {
            Simulation.Start();
            return new NoContentResult();
        }

        public virtual async Task<IActionResult> OnPostStopAsync()
        {
            Simulation.Stop();
            return new NoContentResult();
        }
    }
}