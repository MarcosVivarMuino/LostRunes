using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool<T>
{
    T Get();          // M�todo para obtener un objeto del pool
    void Return(T obj); // M�todo para devolver un objeto al pool
}
