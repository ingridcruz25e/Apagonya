using Google.Cloud.Firestore; namespace UserHub.Models;
[FirestoreData] public class Zone { [FirestoreProperty] public string Id{get;set;}=""; [FirestoreProperty] public string Nombre{get;set;}=""; }
