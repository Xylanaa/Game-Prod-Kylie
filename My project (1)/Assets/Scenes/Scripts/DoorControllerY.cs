using UnityEngine;
using UnityEngine.InputSystem; 

public class DoorControllerY : MonoBehaviour
{
    public GameObject DoorY;          // Het parent-object "DoorY" dat de deur zelf bevat
    public GameObject Doorknob;       // Het child-object "Doorknob" voor de deurknop
    public float openPosition = 5f;   // De positie waar de deur open gaat (verplaatsen langs de Z-as)
    public float doorSpeed = 2f;      // De snelheid waarmee de deur schuift
    private bool isOpen = false;      // Houdt bij of de deur open of dicht is
    private bool isNear = false;      // Houdt bij of de speler in de buurt is
    private Vector3 closedPosition;   // De beginpositie van de deur (gesloten toestand)
    private Vector3 openPositionVector; // De positie van de deur wanneer deze open is
    private float openTime = 0f;      // Tijd dat de deur al open is (in seconden)
    public float autoCloseTime = 4f;  // Tijd (in seconden) voordat de deur automatisch sluit

    // Declareer een InputAction voor de 'E' toets
    private InputAction openCloseAction;

    void Awake()
    {
        // Maak een nieuwe InputAction voor de 'E' toets (bind de E toets op de actie)
        openCloseAction = new InputAction("OpenClose", binding: "<Keyboard>/e");
        openCloseAction.Enable();  // Zorg ervoor dat de actie is ingeschakeld
    }

    void Start()
    {
        // Sla de beginpositie van de deur op (gesloten staat)
        closedPosition = DoorY.transform.position;
        openPositionVector = new Vector3(closedPosition.x, closedPosition.y, closedPosition.z - openPosition); // De open positie langs de Z-as
        
        // Zorg ervoor dat de deurknop altijd zichtbaar is en aan de deur hangt
        Doorknob.transform.SetParent(DoorY.transform);  // Maak de deurknop een child van de deur
        Doorknob.SetActive(true);  // De deurknop is altijd zichtbaar, ongeacht de deurstatus
    }

    void Update()
    {
        // Alleen als de speler in de buurt is, kan de deur openen of sluiten
        if (isNear && openCloseAction.triggered)  // Check of de 'E' toets is ingedrukt
        {
            // Zet de staat van de deur om (open / dicht)
            isOpen = !isOpen;
            openTime = 0f;  // Reset de tijd wanneer de deur weer wordt geopend
        }

        // Verschuif de deur naar de open of gesloten positie
        if (isOpen)
        {
            // Verschuif de deur naar de open positie langs de Z-as
            DoorY.transform.position = Vector3.MoveTowards(DoorY.transform.position, openPositionVector, Time.deltaTime * doorSpeed);

            // Houd de tijd bij dat de deur open is
            openTime += Time.deltaTime;

            // Als de deur langer dan autoCloseTime open is, sluit deze dan automatisch
            if (openTime >= autoCloseTime)
            {
                isOpen = false;  // Sluit de deur automatisch na de ingestelde tijd
            }
        }
        else
        {
            // Verschuif de deur terug naar de gesloten positie langs de Z-as
            DoorY.transform.position = Vector3.MoveTowards(DoorY.transform.position, closedPosition, Time.deltaTime * doorSpeed);
        }
    }

    // Detecteer wanneer de speler dichtbij de deur is
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;  // Zet 'isNear' op true als de speler dichtbij is
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;  // Zet 'isNear' op false als de speler niet meer in de buurt is
        }
    }

    // Vergeet niet om de InputAction te deactiveren om geheugenlekken te voorkomen
    void OnDisable()
    {
        openCloseAction.Disable();
    }
}
