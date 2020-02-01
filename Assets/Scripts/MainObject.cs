using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class MainObject : MonoBehaviour
{
    public static int defaultDuration = 2;
    public Dictionary<Task, GeneralTask[]> allTasks = new Dictionary<Task, GeneralTask[]>();
    public LocationManager locationManager { get; set; }
    public event Action<OurEvent, GameObject> onSomethingHappened;

    public List<BlockList> AllBlockLists = new List<BlockList>();

    [SerializeField]
    public GameObject blockObjPrefab;

    [SerializeField]
    public GameObject resultDialogPrefab;


    // Start is called before the first frame update
    void Start()
    {
        this.InitiateAllTasks();
    }

    // Update is called once per frame
    void Update()
    {
        if (locationManager != null)
        {
            locationManager.CheckAll();
        }

        bool allListsFinished = true;
        foreach (BlockList list in AllBlockLists)
        {
            if (!list.IsFinished())
            {
                allListsFinished = false;
            }
        }

        if (allListsFinished)
        {
            ShowResultDialog();
        }
    }

    void ShowResultDialog()
    {
        GameObject obj = Instantiate(resultDialogPrefab);
        TextMeshPro title = obj.transform.Find("Title").GetComponent<TextMeshPro>();
        TextMeshPro desc = obj.transform.Find("Desc").GetComponent<TextMeshPro>();

        UnityEngine.Object[] chars = GameObject.FindObjectsOfType(typeof(MainCharacter));
        MainCharacter char1 = (MainCharacter)chars[0];
        MainCharacter char2 = (MainCharacter)chars[1];

        Debug.Log("Happines1:" + char1.Happiness.ToString());
        Debug.Log("Happines2:" + char2.Happiness.ToString());

        if (char1.Happiness > 80 && char2.Happiness > 80)
        {
            title.SetText("Good job!");
            desc.SetText("deeeeesc");
        }
        else
        {
            title.SetText("Not good enough");
            desc.SetText("You need to put more effore in this relationship buddy");
        } // TODO

    }

    void Awake()
    {
        locationManager = new LocationManager();
    }

    public static MainObject Get()
    {
        MainObject mainObject = null;
        GameObject MO = GameObject.Find("MO");
        if (MO != null)
        {
            mainObject = MO.GetComponent<MainObject>();
        }
        return mainObject;
    }

    public void InvokeEvent(OurEvent? eventToInvoke, GameObject invoker)
    {
        if (this.onSomethingHappened != null && eventToInvoke != null)
        {
            this.onSomethingHappened(eventToInvoke.Value, invoker);
        }
    }

    public void InitiateAllTasks()
    {
        //Dishes
        allTasks.Add(Task.DO_DISHES, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("sink"), OurEvent.GO_TO_SINK, OurEvent.DO_NOTHING, defaultDuration),
            new ConditionalTask(Sink.CheckSink, Task.WASH_DISHES, null, OurEvent.SAY_TYPICAL, null)
        });

        allTasks.Add(Task.WASH_DISHES, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("sink"), OurEvent.DISHES_START, OurEvent.DISHES_STOP, defaultDuration)
        });

        //Shower
        allTasks.Add(Task.SHOWER, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("shower_entrance"), OurEvent.GO_TO_SHOWER, OurEvent.DO_NOTHING, defaultDuration),
            new ConditionalTask(Shower.IsTaken, Task.USE_SHOWER, null, OurEvent.SHOWER_IS_TAKEN, OurEvent.SAY_TYPICAL)
        });

        allTasks.Add(Task.USE_SHOWER, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("shower"), OurEvent.USE_SHOWER_START, OurEvent.USE_SHOWER_STOP, defaultDuration)
        });

        //Make Bed
        allTasks.Add(Task.MAKE_BED, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("Bed"), OurEvent.MAKE_BED_START, OurEvent.MAKE_BED_STOP, defaultDuration)
        });

        //Sleep
        allTasks.Add(Task.SLEEP, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("Bed"), OurEvent.SLEEP_START, OurEvent.SLEEP_STOP, 0)
        });

        //Laundry
        allTasks.Add(Task.DO_LAUNDRY, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("laundry_basket"), OurEvent.TAKE_CLOTHES, OurEvent.DO_NOTHING, 0),
            new ConditionalTask(LaundryBasket.HasClothes, Task.LAUNDER, Task.LAUNDER, OurEvent.SAY_ANGRY, null)
        });

        allTasks.Add(Task.LAUNDER, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("laundry_machine"), OurEvent.LAUNDRY_START, OurEvent.LAUNDRY_STOP, 10)
        });

        //Eat
        allTasks.Add(Task.EAT, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("kitchen_counter"), OurEvent.GET_FOOD_START, OurEvent.GET_FOOD_STOP, defaultDuration),
            new ConditionalTask(Counter.hasCookedFood, Task.EAT_GOOD_FOOD, Task.EAT_BAD_FOOD, OurEvent.EAT_COUNTER_FOOD, OurEvent.SAY_ANGRY)
        });

        allTasks.Add(Task.EAT_GOOD_FOOD, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("table"), OurEvent.EAT_START, OurEvent.EAT_STOP, defaultDuration)
        });

        allTasks.Add(Task.EAT_BAD_FOOD, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("fridge"), OurEvent.FRIDGE_START, OurEvent.FRIDGE_STOP, defaultDuration), //need to change to fridge
            new SingleMove(Utils.getPositionByName("table"), OurEvent.EAT_START, OurEvent.EAT_STOP, defaultDuration)

        });

        //Toilet
        allTasks.Add(Task.RAISE_TOILET_SEAT, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("Toilet_Down"), OurEvent.RAISE_TOILET_SEAT, OurEvent.DO_NOTHING, defaultDuration)
        });

        allTasks.Add(Task.LOWER_TOILET_SEAT, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("Toilet_Down"), OurEvent.LOWER_TOILET_SEAT, OurEvent.DO_NOTHING, defaultDuration)
        });

        allTasks.Add(Task.USE_TOILET_HER, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("bathroom_entrance"), OurEvent.GO_TO_BATHROOM, OurEvent.DO_NOTHING, defaultDuration),
            new ConditionalTask(Toilet.IsTaken, Task.ACTUALLY_USE_TOILET_HER, null, null, OurEvent.SAY_ANGRY)
        });

        allTasks.Add(Task.USE_TOILET_HIM, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("bathroom_entrance"), OurEvent.GO_TO_BATHROOM, OurEvent.DO_NOTHING, defaultDuration),
            new ConditionalTask(Toilet.IsTaken, Task.ACTUALLY_USE_TOILET_HIM, null, null, OurEvent.SAY_ANGRY)
        });

        allTasks.Add(Task.ACTUALLY_USE_TOILET_HER, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("Toilet_Down"), OurEvent.USE_BATHROOM_START, OurEvent.USE_BATHROOM_STOP, defaultDuration)
        });

        allTasks.Add(Task.ACTUALLY_USE_TOILET_HIM, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("Toilet_Down"), OurEvent.USE_BATHROOM_START, OurEvent.USE_BATHROOM_STOP, defaultDuration)
        });

        //Cook
        allTasks.Add(Task.COOK, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("kitchen_counter"), OurEvent.COOK_START, OurEvent.COOK_STOP, defaultDuration)
        });

        //Water Flower
        allTasks.Add(Task.WATER_FLOWER, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("flowers"), OurEvent.WATER_FLOWER_START, OurEvent.WATER_FLOWER_STOP, defaultDuration)
        });
        // Water Plant
        allTasks.Add(Task.WATER_PLANT, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("plant"), OurEvent.WATER_PLANT_START, OurEvent.WATER_PLANT_STOP, defaultDuration)
        });

        // Clean Shoes
        allTasks.Add(Task.CLEAN_SHOES, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("shoes"), OurEvent.CLEAN_SHOES_START, OurEvent.CLEAN_SHOES_STOP, defaultDuration)
        });
        // Read Book
        allTasks.Add(Task.READ_BOOK, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("book"), OurEvent.TAKE_BOOK_START, OurEvent.TAKE_BOOK_STOP, 0),
            new SingleMove(Utils.getPositionByName("desk"), OurEvent.READ_BOOK_START, OurEvent.READ_BOOK_STOP, defaultDuration)
        });

        // Watch TV
        allTasks.Add(Task.WATCH_TV, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("couch"), OurEvent.WATCH_TV_START, OurEvent.WATCH_TV_STOP, defaultDuration)
        });

        // Clean Coffee Table
        allTasks.Add(Task.CLEAN_COFFEE_TABLE, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("coffee_table"), OurEvent.CLEAN_COFFEE_TABLE_START, OurEvent.CLEAN_COFFEE_TABLE_STOP, defaultDuration)
        });

        // Clean Dining Table
        allTasks.Add(Task.CLEAN_DINING_TABLE, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("table"), OurEvent.CLEAN_DINING_TABLE_START, OurEvent.CLEAN_DINING_TABLE_STOP, defaultDuration)
        });

        // Clean Makeup Table
        allTasks.Add(Task.CLEAN_MAKEUP_TABLE, new GeneralTask[]{
            new SingleMove(Utils.getPositionByName("makeup_table"), OurEvent.CLEAN_MAKEUP_TABLE_START, OurEvent.CLEAN_MAKEUP_TABLE_STOP, defaultDuration)
        });

    }
}