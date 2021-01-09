namespace ChatApp.API
{
    /// <summary>
    /// String Model
    /// </summary>
    public class StringModel<T>
    {
        public string Model { get; set; }
        public T RequestModel { get; set; }
    }
     
}
