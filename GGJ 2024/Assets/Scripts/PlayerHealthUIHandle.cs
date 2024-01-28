using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHealthUIHandle : MonoBehaviour
{
    //Player 1
    public Slider HealthSlider;
    public TextMeshProUGUI BounceText;

    //Player 2
    public Slider HealthSliderTwo;
    public TextMeshProUGUI BounceTextTwo;

    //Player 3
    public Slider HealthSliderThree;
    public TextMeshProUGUI BounceTextThree;

    //Player 4
    public Slider HealthSliderFour;
    public TextMeshProUGUI BounceTextFour;

    public TextMeshProUGUI EventDisplay;


    private Player _player1 = null;
    private Player _player2 = null;
    private Player _player3 = null;
    private Player _player4 = null;


    public float Cooldown = 7;
    private float _counter = 30;

    private void Start()
    {

        HealthSlider.gameObject.SetActive(false);
        HealthSliderTwo.gameObject.SetActive(false);
        HealthSliderThree.gameObject.SetActive(false);
        HealthSliderFour.gameObject.SetActive(false);

        BounceText.gameObject.SetActive(false);
        BounceTextTwo.gameObject.SetActive(false);
        BounceTextThree.gameObject.SetActive(false);
        BounceTextFour.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_player1 != null)
        {
            HealthSlider.value = _player1.Health;
            BounceText.text = string.Format("{0,2:P2}", _player1.BouncePercent.ToString());
        }

        if (_player2 != null)
        {
            HealthSliderTwo.value = _player2.Health;
            BounceTextTwo.text = string.Format("{0,12:P2}", _player2.BouncePercent.ToString());
        }

        if (_player3 != null)
        {
            HealthSliderThree.value = _player3.Health;
            BounceTextThree.text = string.Format("{0,2:P2}", _player3.BouncePercent.ToString());
        }

        if (_player4 != null)
        {
            HealthSliderFour.value = _player4.Health;
            BounceTextFour.text = string.Format("{0,12:P2}", _player4.BouncePercent.ToString());
        }

        if (_counter <= 0)
        {
            EventDisplay.text = "";

            _counter = Cooldown;
        }

        _counter -= Time.deltaTime;
    }


    public void AddPlayer(PlayerInput pPlayer)
    {
        if (_player1 == null)
        { 
            _player1 = pPlayer.GetComponent<Player>();

            HealthSlider.gameObject.SetActive(true);
            BounceText.gameObject.SetActive(true);

            HealthSlider.maxValue = _player1.MaxHealth;
            HealthSlider.value = _player1.Health;
            BounceText.text = string.Format("{0:P2}", _player1.BouncePercent.ToString());

            EventDisplay.text = "Player 1 Joined";
        }
        else if (_player2 == null)
        { 
            _player2 = pPlayer.GetComponent<Player>();

            HealthSliderTwo.gameObject.SetActive(true);
            BounceTextTwo.gameObject.SetActive(true);

            HealthSliderTwo.maxValue = _player2.MaxHealth;
            HealthSliderTwo.value = _player2.Health;
            BounceTextTwo.text = string.Format("{0:P2}", _player2.BouncePercent.ToString());
            EventDisplay.text = "Player 2 Joined";
        }
        else if (_player3 == null)
        {
            _player3 = pPlayer.GetComponent<Player>();

            HealthSliderThree.gameObject.SetActive(true);
            BounceTextThree.gameObject.SetActive(true);

            HealthSliderThree.maxValue = _player3.MaxHealth;
            HealthSliderThree.value = _player3.Health;
            BounceTextThree.text = string.Format("{0:P2}", _player3.BouncePercent.ToString());
            EventDisplay.text = "Player 3 Joined";
        }
        else
        {
            _player4 = pPlayer.GetComponent<Player>();

            HealthSliderFour.gameObject.SetActive(true);
            BounceTextFour.gameObject.SetActive(true);

            HealthSliderFour.maxValue = _player4.MaxHealth;
            HealthSliderFour.value = _player4.Health;
            BounceTextFour.text = string.Format("{0:P2}", _player4.BouncePercent.ToString());

            EventDisplay.text = "Player 4 Joined";
        }
    }

    public void RemovePlayer(PlayerInput pPlayer)
    {
        if (pPlayer.GetComponent<Player>() == _player1)
        {
            _player1 = null;
            HealthSlider.gameObject.SetActive(false);
            BounceText.gameObject.SetActive(false);


            EventDisplay.text = "Player 1 Was Removed";
        }

        if (pPlayer.GetComponent<Player>() == _player2)
        {
            _player2 = null;
            HealthSliderTwo.gameObject.SetActive(false);
            BounceTextTwo.gameObject.SetActive(false);

            EventDisplay.text = "Player 2 Was Removed";
        }

        if (pPlayer.GetComponent<Player>() == _player3)
        {
            _player3 = null;
            HealthSliderThree.gameObject.SetActive(false);
            BounceTextThree.gameObject.SetActive(false);

            EventDisplay.text = "Player 3 Was Removed";
        }

        if (pPlayer.GetComponent<Player>() == _player4)
        {
            _player4 = null;
            HealthSliderFour.gameObject.SetActive(false);
            BounceTextFour.gameObject.SetActive(false);

            EventDisplay.text = "Player 4 Was Removed";
        }
    }
}
