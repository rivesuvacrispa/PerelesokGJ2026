using Global;

namespace Story.Events
{
    public class SignLetterReadFinishEvent : SaveMemoryEvent
    {
        protected override void OnFire()
        {
            GlobalDefinitions.SignLetter.Burn();
        }
    }
}