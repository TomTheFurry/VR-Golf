using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayfieldSubSection : MonoBehaviour
{
    Playfield Playfield;

    void Start() {
        Playfield = GetComponentInParent<Playfield>();
    }

    public void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<IPlayfieldTracked>(out var tracked)) {
            Playfield.NotifyTrackedEnterSubSection(tracked);
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.TryGetComponent<IPlayfieldTracked>(out var tracked)) {
            Playfield.NotifyTrackedExitSubSection(tracked);
        }
    }

}
