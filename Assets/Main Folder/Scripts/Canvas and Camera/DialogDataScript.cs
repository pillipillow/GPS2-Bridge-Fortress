using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDataScript
{
	public static string[] level01 = 
	{
		"Squire",

		"Ouch… We seemed to have fallen pretty far…",

		"Squire",

		"Where’s the Knight? Gosh darn it, we must’ve gotten separated on the way down. That’s not good, the Knight’s going to charge into everything that’s not a wall!",

		"Squire",

		"Please tell me he’s somewhere nearby, please tell me he’s not gotten us into more trouble.",

		"Knight",

		"I’m down here m’lady!",

		"Squire",

		"Oh, there he is and… Thank all things holy, he’s trapped.",

		"Squire",

		"Now, how should we get out?",

		"Squire",

		"Those bridges down there seem like they could be turned… and there are these cranks up here. Maybe they were part of the defense mechanics before it was abandoned?",

		"Squire",

		"To lead enemies around into traps while allowing their own troops the more advantageous position.",

		"Squire",

		"… I should be able to do the same. Alright, let’s try these out."
	};

	public static string[] level02 = 
	{
		"Knight",

		"Oh-hoh! An arrow trap!",

		"Knight",

		"You know what to do from your training Squire! A shield can protect a man from anything. Toss away!",

		"Squire",
	
		"Almost anything’, sir. Not absolutely anything... There doesn’t seem to be a way down, so I guess I really will be tossing them",

		"Squire",

		"I don’t know how many levels we have to pass through before we reach the exit of the fortress though, so it would be best to ration the shield usage to one per level, just in case.",

		"Squire",

		"Because I’m definitely not getting them back."

	};

	public static string[] level03 = 
	{
		"Squire",

		"A Goblin! Did they decide to live here after finding that it was empty?",

		"Squire",

		"Goblins are strong, they would be able to break these wooden shields no problem…",

		"Squire",

		"If they see the Knight, there’s no doubt they would give chase. But I should be able to direct them away, especially into all these traps here.",

		"Knight",

		"Come at me, foul creatures! None of you shall stand come the morrow!",

		"Squire",

		"... Sir, do reconsider, even you would not last against enemies at every corner.",

		"Squire",

		"That goes for traps too… though some traps are quite unavoidable-",

		"Knight",

		"You jest, Squire, hohoho! I’ll not be undone by mere greenskins and needles!",

		"Squire",

		"Those are clearly not needles. The shields could work as a means to move over traps unharmed and even the Knight would be smart enough to pick them up for use later… But that may also go for the goblins around.",

		"Squire",

		"But that may also go for the goblins around.",

		"Squire",

		"If anything goes wrong however, I could use these Flash Bombs. They’ll stun anyone looking at it, even the Knight in a pinch so I have more time to get to a crank or something.",
	};

	public static string[] level04 =
	{
		"Squire",

		"New traps… Perhaps it would be wise to observe that Goblin there before making any moves."
	};

	public static string[] level05 =
	{

		"Knight",

		"The thieves! They’ve stolen armor from the barracks, I’ll not let a fellow knight’s armor be defiled in your hands!",

		"Squire",

		"Oh no, with those on, they would need an extra hit to defeat.",

		"Squire",

		"I’ll have to be careful, the Knight wouldn’t think twice about attacking."

	};


	//HT empty for now, do not remove.
	public static string[] level06 =
	{

	};

	//HT empty for now, do not remove.
	public static string[] level07 =
	{

	};
	
	public static string[] level08 =
	{
		"Knight", 

		"Hah, you send bugs to fend against me, fortress? I would merely need to cover the holes in my helmet, and then you could do nothing else.",

		"Squire",

		"That also means you can’t see, sir!",

		"Knight",

		"Then I would only have to stop, is it not? Hohoho!",

		"Squire",

		"Sigh.. Well, as for the fire, they were a pretty popular trap back home",

		"Squire",

		"They always used pressure plates to activate them too so there should be some around here as well.",

		"Squire",

		"... The Knight should turn around when faced with the heat of the fires, so I shouldn’t have to worry about him roasting himself.",

		"Squire",

		"The bugs though, probably aren’t that smart and I could always also throw a Flash Bang to scatter them."

	};

	public static string[] level09 =
	{

	};

	public static string[] level10 =
	{
		
	};

	public static string[] level11 =
	{
		
	};

	public static string[] level12 =
	{
		"Squire",

		"Brr… What’s this chill down my spine?",

		"Squire",

		"Ghosts?",

		"Knight", 

		"Wait, wait…. there are….ghosts !? There is nothing to attack, no glory of battle",

		"Squire",

		"If he can’t hit something that is a phantom, he would just turn and run !",

		"Squire",

		"I doubt they’ll conveniently trigger any traps either.",

		"Squire",

		"But they’re sticking to the paths, perhaps because they used to be human?",

		"Squire",

		"Reasoning aside, I can definitely exploit that",

		"Squire",

		"Those shaking spots seems suspicious too, we should probably avoid them"
	};

	public static string[] level13 =
	{

	};

	public static string[] level14 =
	{
		"Squire",

		" !... ",

		"Squire",

		"Oh God, that was a dragon.",

		"Knight",

		"A dragon, Squire! Onwards, today we shall bring glory to our names in a battle with this threat!",

		"Squire", 

		"Definitely not, we are not prepared to fight such a thing! What in the world made this fortress so inviting for every hostile creature under the sun?!",

		"Squire", 

		"I can see frost along some of the edges of the platforms and bridges. That’s most likely it’s attack zone.",

		"Knight" ,

		"Hah hah, do not fret for dragons are slow! We shall attack while it prepares!",

		"Squire" ,

		"I’m keeping the Knight out of it when I start seeing the energy build, anything caught in that blast would not survive."

	};

	public static string[] level15 =
	{

	};

	public static string[] level16 =
	{

	};

	public static string[] level17 =
	{

	};

	public static string[] level18 =
	{

	};


	public static List<string[]> levelDialogs = new List<string[]>
	{level01, level02, level03, level04, level05, level06, level07, level08, level09, level10, level11, level12, level13, level14, level15, level16, level17, level18};

}
