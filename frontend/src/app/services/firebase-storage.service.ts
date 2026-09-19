import { Injectable } from '@angular/core';
import { getApp, getApps, initializeApp } from 'firebase/app';
import { getDownloadURL, getStorage, ref, uploadBytes } from 'firebase/storage';
import { firebaseConfig } from '../config/firebase.config';

@Injectable({providedIn:'root'})
export class FirebaseStorageService {
  private storage;
  constructor(){
    const app=getApps().length?getApp():initializeApp(firebaseConfig);
    this.storage=getStorage(app);
  }
  async upload(file:File):Promise<string>{
    const path=`reportes/${Date.now()}-${file.name}`;
    const fileRef=ref(this.storage,path);
    await uploadBytes(fileRef,file);
    return getDownloadURL(fileRef);
  }
}
