using System;
using System.Text;

namespace hashes;

public class GhostsTask :
    IFactory<Document>,
    IFactory<Vector>,
    IFactory<Segment>,
    IFactory<Cat>,
    IFactory<Robot>,
    IMagic
{
    private object _ghostItem;
    private readonly byte[] _documentContent = [1, 2, 3];
    private readonly DateTime _catBirthDate = DateTime.Now;

    public void DoMagic()
    {
        switch (_ghostItem)
        {
            case Vector vector:
                vector.Add(new Vector(1, 1));
                break;
            case Segment segment:
                segment.End.Add(new Vector(1, 1));
                break;
            case Document:
                _documentContent[0] = 67;
                break;
            case Cat cat:
                cat.Rename("Жора");
                break;
            case Robot:
                Robot.BatteryCapacity++;
                break;
        }
    }

    Vector IFactory<Vector>.Create()
    {
        _ghostItem ??= new Vector(0, 0);

        return _ghostItem as Vector;
    }

    Segment IFactory<Segment>.Create()
    {
        _ghostItem ??= new Segment(new Vector(0, 0), new Vector(0, 0));

        return _ghostItem as Segment;
    }

    Document IFactory<Document>.Create()
    {
        _ghostItem ??= new Document("meow", Encoding.UTF8, _documentContent);

        return _ghostItem as Document;
    }

    Cat IFactory<Cat>.Create()
    {
        _ghostItem ??= new Cat("Морти", "Дворовая", _catBirthDate);

        return _ghostItem as Cat;
    }

    Robot IFactory<Robot>.Create()
    {
        _ghostItem ??= new Robot("zero", 100.0);

        return _ghostItem as Robot;
    }
}