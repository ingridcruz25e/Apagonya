using Google.Cloud.Firestore; namespace UserHub.Models;
[FirestoreData] public class Resolution { [FirestoreProperty] public string Id{get;set;}=""; [FirestoreProperty] public string ReporteId{get;set;}=""; [FirestoreProperty] public string Causa{get;set;}=""; [FirestoreProperty] public DateTime HoraRestablecimiento{get;set;} [FirestoreProperty] public string Detalle{get;set;}=""; }
