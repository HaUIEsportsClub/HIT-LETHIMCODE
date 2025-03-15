namespace _Scripts.UI
{
    public class UIController : Singleton<UIController>
    {
        public UISetting UISetting => FindObjectOfType<UISetting>();
        
        protected override void Awake()
        {
            base.KeepAlive(false);
            base.Awake();
        }
    }
}