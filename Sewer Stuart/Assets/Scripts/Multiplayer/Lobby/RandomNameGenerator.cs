using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomNameGenerator : MonoBehaviour
{
    [SerializeField] InputField playerName;

    string[] first = new string[]
    {
        "sewer", "absurd", "amusing", "entertaining", "hilarious", "ludicrous", "playful", "ridiculous", "adorable", "adventurous", "aggressive", "agreeable",
        "alert", "alive", "amused", "angry", "annoyed", "annoying", "anxious", "arrogant", "ashamed", "attractive", "average", "awful", "bad", "beautiful",
        "bewildered", "bloody", "ugly", "smelly", "blushing", "bored", "brainy", "brave", "unbreakable", "bright", "busy", "calm", "careful", "cautious",
        "charming", "cheerful", "clean", "clear", "clever", "cloudy", "clumsy", "colorful", "combative", "comfortable", "concerned", "condemned", "confused",
        "cooperative", "courageous", "crazy", "creepy", "crowded", "cruel", "curious", "cute", "dangerous", "dark", "dead", "defeated", "defiant", "delightful",
        "depressed", "determined", "disgusted", "disturbed", "dizzy", "doubtful", "dull", "giga", "mega", "long", "crispy", "mister", "supreme", "immortal",
        "spider", "iron", "captain", "god", "strange", "destroyer", "fast", "lucky", "absolute", "fantastic", "kung-fu", "chonky"
    };

    string[] last = new string[]
    {
        "stuart", "dover", "johnson", "wigglesworth", "jackson", "chad", "peterson", "buck", "hector", "karen", "kyle", "guy", "bacon", "mister", "dr.", "child",
        "drinker", "leader", "dude", "person", "worker", "mom", "dad", "king", "queen", "friend", "plant", "animal", "officer", "skin", "log", "face", "buddy",
        "god", "goddess", "deity", "celestial", "creator", "man", "lord", "knight", "adam", "thing", "captain", "witch", "mutant", "brick", "unit", "chonker",
        "rat", "mouse", "squirrel", "dog", "cat", "fox", "sheep", "pig", "cow", "horse", "panda", "bear", "gorilla", "frog", "dolphin", "snake", "fish", "giraffe", 
        "angela", "brendan", "darren", "dawid", "david", "kristina", "roberto", "ryan", "ugnius"
    };

    public void GenerateRandomName()
    {
        int randFirst = Random.Range(0, first.Length);
        int randLast = Random.Range(0, last.Length);

        char[] firstName = first[randFirst].ToCharArray();
        firstName[0] = char.ToUpper(firstName[0]);

        char[] lastName = last[randLast].ToCharArray();
        lastName[0] = char.ToUpper(lastName[0]);

        playerName.text = new string(firstName) + " " + new string(lastName);
    }
}
