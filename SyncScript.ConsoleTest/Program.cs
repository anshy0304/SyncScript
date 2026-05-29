using SyncScript.Core;

var doc = new CRDTDocument();

// We'll simulate typing "Hello"
// Each character needs a unique Id and points to the previous character as its parent

var h = new CRDTNode(Guid.NewGuid(), null, 'H', "user-ansh");
var e = new CRDTNode(Guid.NewGuid(), h.Id, 'e', "user-ansh");
var l = new CRDTNode(Guid.NewGuid(), e.Id, 'l', "user-ansh");
var l2 = new CRDTNode(Guid.NewGuid(), l.Id, 'l', "user-ansh");
var o = new CRDTNode(Guid.NewGuid(), l2.Id, 'o', "user-ansh");

doc.Insert(h);
doc.Insert(e);
doc.Insert(l);
doc.Insert(l2);
doc.Insert(o);

Console.WriteLine("After insert:  " + doc.GetText()); // should print: Hello

// Now delete 'e' (tombstone it, not remove it)
doc.Delete(e.Id);

Console.WriteLine("After delete:  " + doc.GetText()); // should print: Hllo

// Conflict test — two users insert at the same position (same ParentId)
var doc2 = new CRDTDocument();

var a = new CRDTNode(Guid.Parse("aaaaaaaa-0000-0000-0000-000000000000"), null, 'A', "user-ansh");
var b = new CRDTNode(Guid.Parse("bbbbbbbb-0000-0000-0000-000000000000"), a.Id, 'B', "user-ansh");
var x = new CRDTNode(Guid.Parse("cccccccc-0000-0000-0000-000000000000"), a.Id, 'X', "user-b");
doc2.Insert(a);
doc2.Insert(b); // user-ansh types 'B' after 'A'
doc2.Insert(x); // user-b also types 'X' after 'A' — conflict!

Console.WriteLine("Conflict resolved: " + doc2.GetText()); // always: ABX or AXB — same on both machines
var doc3 = new CRDTDocument();

var n1 = new CRDTNode(Guid.NewGuid(), null, 'H', "user-ansh");
var n2 = new CRDTNode(Guid.NewGuid(), n1.Id, 'i', "user-ansh");

doc3.Apply(new CRDTOperation(OperationType.Insert, n1, "doc-1"));
doc3.Apply(new CRDTOperation(OperationType.Insert, n2, "doc-1"));
Console.WriteLine("Apply insert: " + doc3.GetText()); // Hi

doc3.Apply(new CRDTOperation(OperationType.Delete, n1, "doc-1"));
Console.WriteLine("Apply delete: " + doc3.GetText()); // i