using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

/// <summary>
/// Расширенный компонент для спавна объектов с поддержкой масштабирования.
/// Работает вместе с ObjectSpawner, добавляя возможность задавать начальный масштаб объектов.
/// </summary>
[RequireComponent(typeof(ObjectSpawner))]
public class ObjectSpawnerWithScale : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Использовать ли масштабирование при спавне объектов.")]
        bool m_UseScale = true;

        /// <summary>
        /// Использовать ли масштабирование при спавне объектов.
        /// </summary>
        public bool useScale
        {
            get => m_UseScale;
            set => m_UseScale = value;
        }

        [SerializeField]
        [Tooltip("Начальный масштаб объектов (если Use Random Scale выключен).")]
        Vector3 m_BaseScale = Vector3.one;

        /// <summary>
        /// Начальный масштаб объектов (если Use Random Scale выключен).
        /// </summary>
        public Vector3 baseScale
        {
            get => m_BaseScale;
            set => m_BaseScale = value;
        }

        [SerializeField]
        [Tooltip("Использовать случайный масштаб в заданном диапазоне.")]
        bool m_UseRandomScale = false;

        /// <summary>
        /// Использовать случайный масштаб в заданном диапазоне.
        /// </summary>
        public bool useRandomScale
        {
            get => m_UseRandomScale;
            set => m_UseRandomScale = value;
        }

        [SerializeField]
        [Tooltip("Минимальный масштаб для случайного выбора (используется только если Use Random Scale включен).")]
        Vector3 m_MinScale = new Vector3(0.5f, 0.5f, 0.5f);

        /// <summary>
        /// Минимальный масштаб для случайного выбора.
        /// </summary>
        public Vector3 minScale
        {
            get => m_MinScale;
            set => m_MinScale = value;
        }

        [SerializeField]
        [Tooltip("Максимальный масштаб для случайного выбора (используется только если Use Random Scale включен).")]
        Vector3 m_MaxScale = new Vector3(2f, 2f, 2f);

        /// <summary>
        /// Максимальный масштаб для случайного выбора.
        /// </summary>
        public Vector3 maxScale
        {
            get => m_MaxScale;
            set => m_MaxScale = value;
        }

        [SerializeField]
        [Tooltip("Сохранять пропорции при случайном масштабировании (использует только X значение для всех осей).")]
        bool m_UniformScale = true;

        /// <summary>
        /// Сохранять пропорции при случайном масштабировании.
        /// </summary>
        public bool uniformScale
        {
            get => m_UniformScale;
            set => m_UniformScale = value;
        }

        ObjectSpawner m_ObjectSpawner;

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        void Awake()
        {
            m_ObjectSpawner = GetComponent<ObjectSpawner>();
            if (m_ObjectSpawner != null)
            {
                // Подписываемся на событие спавна объекта
                m_ObjectSpawner.objectSpawned += OnObjectSpawned;
            }
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        void OnDestroy()
        {
            if (m_ObjectSpawner != null)
            {
                m_ObjectSpawner.objectSpawned -= OnObjectSpawned;
            }
        }

        /// <summary>
        /// Обработчик события спавна объекта. Применяет масштабирование к только что созданному объекту.
        /// </summary>
        /// <param name="spawnedObject">Объект, который был только что создан.</param>
        void OnObjectSpawned(GameObject spawnedObject)
        {
            if (!m_UseScale || spawnedObject == null)
                return;

            Vector3 scaleToApply;

            if (m_UseRandomScale)
            {
                if (m_UniformScale)
                {
                    // Используем только X для всех осей, чтобы сохранить пропорции
                    float randomValue = Random.Range(m_MinScale.x, m_MaxScale.x);
                    scaleToApply = new Vector3(randomValue, randomValue, randomValue);
                }
                else
                {
                    // Разные значения для каждой оси
                    scaleToApply = new Vector3(
                        Random.Range(m_MinScale.x, m_MaxScale.x),
                        Random.Range(m_MinScale.y, m_MaxScale.y),
                        Random.Range(m_MinScale.z, m_MaxScale.z)
                    );
                }
            }
            else
            {
                scaleToApply = m_BaseScale;
            }

            // Применяем масштаб напрямую (заменяем текущий масштаб)
            spawnedObject.transform.localScale = scaleToApply;
        }

        /// <summary>
        /// Устанавливает базовый масштаб для всех объектов.
        /// </summary>
        /// <param name="scale">Масштаб для установки.</param>
        public void SetBaseScale(Vector3 scale)
        {
            m_BaseScale = scale;
        }

        /// <summary>
        /// Устанавливает базовый масштаб для всех объектов (равномерное масштабирование).
        /// </summary>
        /// <param name="scale">Масштаб для установки (применяется ко всем осям).</param>
        public void SetBaseScale(float scale)
        {
            m_BaseScale = new Vector3(scale, scale, scale);
        }
    }
