namespace SatisfactoryApp.Services.Machines;

public class MachineFilters
{
    public List<MachineTypeOption> SelectedMachineTypes { get; set; } = [];
    public List<PowerCircuitOption> SelectedPowerCircuits { get; set; } = [];
    public List<MachineStabilityOption> SelectedMachineStabilities { get; set; } = [];
}
