using System.Text.Json;

namespace Roro.Flows.Framework
{
    public abstract class NameTypeValueCollection<TParent, TCollection, TItem>
        : ViewModelCollection<TParent, TCollection, TItem>
        where TParent : ViewModel
        where TCollection : NameTypeValueCollection<TParent, TCollection, TItem>
        where TItem : NameTypeValue<TParent, TCollection, TItem>
    {
        protected NameTypeValueCollection(TParent parent) : base(parent)
        {
        }

        protected NameTypeValueCollection(TParent parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }
    }
}
