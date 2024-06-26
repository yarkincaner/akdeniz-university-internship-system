namespace Internships.Core.Wrappers
{
    public class SelectModelResponse<T>
    {
        public SelectModelResponse(T id, string name)
        {
            Id = id;
            Name = name;
        }
        public T Id { get; private set; }

        public string Name { get; private set; }
    }
}
