using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomExtensions;
using UnityEngine.SceneManagement;
using TMPro;

public class GameBehavior : MonoBehaviour, IManager
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI itemsCollectedText;
    public TextMeshProUGUI winScreenText;
    public TextMeshProUGUI lossScreenText;
    public UnityEngine.UI.Button winButton;
    public UnityEngine.UI.Button lossButton;
    public TextMeshProUGUI lootReportText;
    public delegate void DebugDelegate(string newText);
    public DebugDelegate debug = Print;
    public Stack<string> lootStack = new Stack<string>();
    private string _state;
    public string State
    {
        get { return _state; }
        set { _state = value; }
    }
    public string labelText = "Collect all 4 items and win your freedom!";
    public int maxItems = 4;
    public bool showWinScreen = false;
    public bool showLossScreen = false;
    private int _itemsCollected = 0;
    public int Items
    {
        get { return _itemsCollected; }
        set
        {
            _itemsCollected = value;
            if(_itemsCollected >= maxItems)
            {
                labelText = "You've found all the items!";
                showWinScreen = true;
                Time.timeScale = 0f;
                winScreenText.gameObject.SetActive(true);
                winButton.gameObject.SetActive(true);
            }
            else
            {
                labelText = "Item found, only " + (maxItems - _itemsCollected) + " more to go!";
            }
            Debug.LogFormat("Items: {0}", _itemsCollected);
        }
    }
    public int _playerHP = 3;
    public int HP
    {
        get { return _playerHP; }
        set
        {
            _playerHP = value;
            if(_playerHP <= 0)
            {
                labelText = "That looked like it hurt. Try again?";
                showLossScreen = true;
                Time.timeScale = 0;
                lossScreenText.gameObject.SetActive(true);
                lossButton.gameObject.SetActive(true);
            }
            else
            {
                labelText = "The point is to not get hit.";
            }
            Debug.LogFormat("Lives: {0}", _playerHP);
        }
    }
    void RestartLevel()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
    void Start()
    {
        Initialize();
        winScreenText.gameObject.SetActive(false);
        lossScreenText.gameObject.SetActive(false);
        winButton.gameObject.SetActive(false);
        lossButton.gameObject.SetActive(false);
        InventoryList<string> inventoryList = new InventoryList<string>();
        inventoryList.SetItem("Potion");
        Debug.Log(inventoryList.item);

    }
    public void Initialize()
    {
        _state = "Manager initialized.";
        _state.FancyDebug();
        debug(_state);
        LogWithDelegate(debug);
        GameObject player = GameObject.Find("Player");
        PlayerBehavior playerBehavior = player.GetComponent<PlayerBehavior>();
        playerBehavior.playerJump += HandlePlayerJump;
        Debug.Log(_state);
        lootStack.Push("Sword of Doom");
        lootStack.Push("HP+");
        lootStack.Push("Golden Key");
        lootStack.Push("Winged Boot");
        lootStack.Push("Mythril Bracers");
    }
    public void HandlePlayerJump()
    {
        debug("Player has jumped...");
    }
    public static void Print(string newText)
    {
        Debug.Log(newText);
    }
    public void LogWithDelegate(DebugDelegate del)
    {
        del("Delegating the debug task...");
    }
    void Update()
    {
        healthText.text = "Health: "+_playerHP;
        itemsCollectedText.text = "Keys : "+_itemsCollected;
    }
    public void PrintLootReport()
    {
        var currentItem = lootStack.Pop();
        var nextItem = lootStack.Peek();
        Debug.LogFormat("You got a {0}! You've got a good chance of finding a {1} next!", currentItem, nextItem);
        Debug.LogFormat("There are {0} random loot items waiting for you!", lootStack.Count);
    }
    public void RestartGame ()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }
    public void Menu ()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
}  