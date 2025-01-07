using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool<T>
{
    T Get();          // Método para obtener un objeto del pool
    void Return(T obj); // Método para devolver un objeto al pool
}
