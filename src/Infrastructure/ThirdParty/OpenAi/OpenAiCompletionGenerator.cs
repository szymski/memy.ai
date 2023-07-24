namespace Infrastructure.ThirdParty.OpenAi; 

public class OpenAiCompletionGenerator {
    private readonly OpenAiConfig _config;
    
    public OpenAiCompletionGenerator(OpenAiConfig config)
    {
        _config = config;
    }
}