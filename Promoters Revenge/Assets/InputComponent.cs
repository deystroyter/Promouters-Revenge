using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class InputComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buttonText; // ����� ������
    [SerializeField] private string _defaultKeyName; // ����/��� ������
    [SerializeField] private KeyCode _defaultKeyCode; // ������� �����
    [SerializeField] private Button _keyButton;

    public KeyCode keyCode { get; set; }
    private string tmpKey;

    protected void Start()
    {
        _keyButton.onClick.AddListener(ButtonSetKey);
    }

    public TextMeshProUGUI buttonText
    {
        get { return _buttonText; }
    }

    public string defaultKeyName
    {
        get { return _defaultKeyName; }
    }

    public KeyCode defaultKeyCode
    {
        get { return _defaultKeyCode; }
    }

    public void ButtonSetKey() // ������� ������, ��� �������� � ����� ��������
    {
        tmpKey = _buttonText.text;
        _buttonText.text = "???";

        StartCoroutine(Wait());
    }

    // ����, ����� ����� ������ �����-������ �������, ��� ��������
    // ���� ����� ������ ������� 'Escape', �� ������
    IEnumerator Wait()
    {
        while (true)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _buttonText.text = tmpKey;
                yield break;
            }

            foreach (KeyCode k in KeyCode.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(k) && !Input.GetKeyDown(KeyCode.Escape) && !Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.A) &&
                    !Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.D))
                {
                    keyCode = k;
                    _buttonText.text = k.ToString();
                    yield break;
                }
            }
        }
    }
}