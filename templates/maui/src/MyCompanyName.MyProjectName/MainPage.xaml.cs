using Volo.Abp.DependencyInjection;

namespace MyCompanyName.MyProjectName;

public partial class MainPage : ContentPage, ISingletonDependency
{
	private readonly HelloWorldService _helloWorldService;
	
	int count = 0;

	public MainPage(HelloWorldService helloWorldService)
	{
		_helloWorldService = helloWorldService;
		InitializeComponent();
		SetHelloLabText();

    }

    private void SetHelloLabText()
    {
		HelloLab.Text = _helloWorldService.SayHello();
    }
    
    private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}
