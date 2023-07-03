using LocoSQS.IntegrationTest.Specflow.Support;
using NUnit.Framework;

namespace LocoSQS.E2E.StepDefinitions;

[Binding]
public class TagQueueActionDefinitions
{
    private SharedData _sharedData;

    public TagQueueActionDefinitions(SharedData sharedData)
    {
        _sharedData = sharedData;
    }

    [Given("the queue has a tag with name (.*) and value (.*)")]
    public async Task TagQueue(string name, string value)
    {
        await EnvironmentSetup.CLIENT.TagQueueAsync(new()
        {
            QueueUrl = _sharedData.QueueUrl,
            Tags = new()
            {
                {name, value}
            }
        });
    }

    [When("i remove a tag with name (.*) from the queue")]
    public async Task UntagQueue(string name)
    {
        await EnvironmentSetup.CLIENT.UntagQueueAsync(new()
        {
            QueueUrl = _sharedData.QueueUrl,
            TagKeys = new List<string>()
            {
                name
            }
        });
    }

    [Then("the queue has a tag with name (.*) and value (.*)")]
    public async Task CheckQueueTag(string key, string value)
    {
        Assert.That(await CheckQueueTagExists(key, value), Is.EqualTo(true));
    }
    
    [Then("the queue does not have a tag with name (.*)")]
    public async Task CheckNotQueueTag(string key)
    {
        Assert.That(await CheckQueueTagExists(key, null), Is.EqualTo(false));
    }

    [Then("the queue has (.*) tags?")]
    public async Task CheckTagCount(int count)
    {
        var tags = await EnvironmentSetup.CLIENT.ListQueueTagsAsync(new()
        {
            QueueUrl = _sharedData.QueueUrl
        });
        
        Assert.That(tags.Tags.Count, Is.EqualTo(count));
    }

    private async Task<bool> CheckQueueTagExists(string key, string? value)
    {
        var tags = await EnvironmentSetup.CLIENT.ListQueueTagsAsync(new()
        {
            QueueUrl = _sharedData.QueueUrl
        });

        foreach (var (s, value1) in tags.Tags)
        {
            if (key == s && (value == null || value == value1))
                return true;
        }

        return false;
    }
}