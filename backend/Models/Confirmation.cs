using Google.Cloud.Firestore; namespace UserHub.Models;
[FirestoreData] public class Confirmation { [FirestoreProperty] public string Id{get;set;}=""; [FirestoreProperty] public string ReporteId{get;set;}=""; [FirestoreProperty] public string CiudadanoId{get;set;}=""; [FirestoreProperty] public DateTime Fecha{get;set;} }
