using UnityEngine;

namespace Utils {
    public interface IFactory {
        GameObject CreateGameObject(params object[] parameters);
    }
}