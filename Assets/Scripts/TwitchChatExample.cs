using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TwitchIRC))]
public class TwitchChatExample : MonoBehaviour
{
	public int maxMessages = 100; //we start deleting UI elements when the count is larger than this var.
	private LinkedList<GameObject> messages =
		new LinkedList<GameObject>();
	private TwitchIRC IRC;

	public GameObject UpButton;
	public GameObject DownButton;
	public GameObject LeftButton;
	public GameObject RightButton;
	public GameObject TurnLeftButton;
	public GameObject TurnRightButton;

	public GameObject fightButton;
	//when message is recieved from IRC-server or our own message.
	void OnChatMsgRecieved(string msg)
	{
		//parse from buffer.
		int msgIndex = msg.IndexOf("PRIVMSG #");
		string msgString = msg.Substring(msgIndex + IRC.channelName.Length + 11);
		//string user = msg.Substring(1, msg.IndexOf('!') - 1);

		//remove old messages for performance reasons.
		if (messages.Count > maxMessages)
		{
			Destroy(messages.First.Value);
			messages.RemoveFirst();
		}

		//Debug.Log(msgString);
		string command = "";
		if(msgString.IndexOf(' ') > 0){
			command = msgString.Substring(0, msgString.IndexOf(' ') - 1);
		} else if(msgString.IndexOf(' ') == -1){
			command = msgString;
		}

		if(command.IndexOf('!') != -1 && command.IndexOf('!') == 0){
			if(command.ToLower() == "!click"){
				int val = Random.Range(0,7);
				switch (val) 
				{
					case 0:
					  command = "!up";
					  break;
					case 1:
					  command = "!down";
					  break;
					case 2:
					  command = "!left";
					  break;
					case 3:
					  command = "!right";
					  break;
					case 4:
					  command = "!turnl";
					  break;
					case 5:
					  command = "!turnr";
					  break;
					case 6:
					  command = "!fight";
					  break;
				}
			}


			switch (command.ToLower()) 
			{
				case "!up":
					UpButton.GetComponent<MovePlayerByButton>().PressButton();
				break;
				case "!down":
					DownButton.GetComponent<MovePlayerByButton>().PressButton();
				break;
				case "!left":
					LeftButton.GetComponent<MovePlayerByButton>().PressButton();
				break;
				case "!right":
					RightButton.GetComponent<MovePlayerByButton>().PressButton();
				break;
				case "!turnl":
					TurnLeftButton.GetComponent<MovePlayerByButton>().PressButton();
				break;
				case "!turnr":
					TurnRightButton.GetComponent<MovePlayerByButton>().PressButton();
				break;
				case "!fight":
					fightButton.GetComponent<CombatMenuButton>().Fight();
				break;
			}
		}
		//Debug.Log("Test Command: " + command.IndexOf('!'));

	
		//add new message.
		//CreateUIMessage(user, msgString);
	}
	// void CreateUIMessage(string userName, string msgString)
	// {
	// 	Color32 c = ColorFromUsername(userName);
	// 	string nameColor = "#" + c.r.ToString("X2") + c.g.ToString("X2") + c.b.ToString("X2");
	// 	GameObject go = new GameObject("twitchMsg");
	// 	var text = go.AddComponent<UnityEngine.UI.Text>();
	// 	var layout = go.AddComponent<UnityEngine.UI.LayoutElement>();
	// 	go.transform.SetParent(chatBox);
	// 	messages.AddLast(go);

	// 	layout.minHeight = 20f;
	// 	text.text = "<color=" + nameColor + "><b>" + userName + "</b></color>" + ": " + msgString;
	// 	text.color = Color.black;
	// 	text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
	// 	scrollRect.velocity = new Vector2(0, 1000f);
	// }
	// //when Submit button is clicked or ENTER is pressed.
	// public void OnSubmit()
	// {
	// 	if (inputField.text.Length > 0)
	// 	{
	// 		IRC.SendMsg(inputField.text); //send message.
	// 		CreateUIMessage(IRC.nickName, inputField.text); //create ui element.
	// 		inputField.text = "";
	// 	}
	// }
	// Color ColorFromUsername(string username)
	// {
	// 	Random.seed = username.Length + (int)username[0] + (int)username[username.Length - 1];
	// 	return new Color(Random.Range(0.25f, 0.55f), Random.Range(0.20f, 0.55f), Random.Range(0.25f, 0.55f));
	// }
	// Use this for initialization
	void Start()
	{
		IRC = this.GetComponent<TwitchIRC>();
		//IRC.SendCommand("CAP REQ :twitch.tv/tags"); //register for additional data such as emote-ids, name color etc.
		IRC.messageRecievedEvent.AddListener(OnChatMsgRecieved);
	}
}