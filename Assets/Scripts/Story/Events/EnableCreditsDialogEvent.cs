using Global;

namespace Story.Events
{
    public class EnableCreditsDialogEvent : DialogEvent
    {
        public override void Fire()
        {
            GlobalDefinitions.Credits.gameObject.SetActive(true);
        }
    }
}