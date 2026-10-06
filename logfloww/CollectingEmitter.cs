using System.Collections.Generic;
namespace logfloww;
public class CollectingEmitter<T>
{
    // Yayılan ürünleri saklayacağımız liste
    public List<T> Items { get; } = new List<T>();

    // Yeni gelen nesneyi listeye ekler
    public void Emit(T item)
    {
        Items.Add(item);
    }
}