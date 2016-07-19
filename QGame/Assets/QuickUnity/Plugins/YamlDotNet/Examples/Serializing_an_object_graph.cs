﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System;

using UnityEngine;

using YamlDotNet.Serialization;

public class Serializing_an_object_graph : MonoBehaviour {

    void Start () {
        var address = new Address
        {
            street = "123 Tornado Alley\nSuite 16",
            city = "East Westville",
            state = "KS"
        };

        var receipt = new Receipt
        {
            receipt = "Oz-Ware Purchase Invoice",
            date = new DateTime(2007, 8, 6),
            customer = new Customer
            {
                given = "Dorothy",
                family = "Gale"
            },
            items = new Item[]
            {
                new Item
                {
                    part_no = "A4786",
                    descrip = "Water Bucket (Filled)",
                    price = 1.47M,
                    quantity = 4
                },
                new Item
                {
                    part_no = "E1628",
                    descrip = "High Heeled \"Ruby\" Slippers",
                    price = 100.27M,
                    quantity = 1
                }
            },
            bill_to = address,
            ship_to = address,
            specialDelivery = "Follow the Yellow Brick\n" +
                              "Road to the Emerald City.\n" +
                              "Pay no attention to the\n" +
                              "man behind the curtain."
        };

        var serializer = new Serializer();
        var stringBuilder = new StringBuilder();
        var stringWriter = new StringWriter(stringBuilder);
        serializer.Serialize(stringWriter, receipt);

        Debug.Log(stringBuilder);
    }
}

public class Address {
    public string street { get; set; }
    public string city   { get; set; }
    public string state  { get; set; }
}

public class Receipt {
    public string   receipt         { get; set; }
    public DateTime date            { get; set; }
    public Customer customer        { get; set; }
    public Item[]   items           { get; set; }
    public Address  bill_to         { get; set; }
    public Address  ship_to         { get; set; }
    public string   specialDelivery { get; set; }
}

public class Customer {
    public string given  { get; set; }
    public string family { get; set; }
}

public class Item {
    public string  part_no  { get; set; }
    public string  descrip  { get; set; }
    public decimal price    { get; set; }
    public int     quantity { get; set; }
}
