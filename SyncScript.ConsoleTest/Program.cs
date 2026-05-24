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