import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { CommonSearch } from '../Models/common-search.model';
import { ManagerLevel } from '../Models/ManagerLevel.model';
import { Observable } from 'rxjs';
import { Vendor } from '../Models/common-models';

@Injectable({
  providedIn: 'root'
})
export class VendorService {
  private API_URL:string = `${environment.BASE_URL}/Vendor`;
  constructor(private _http:HttpClient) { }
 
  getVendors(searchModel:CommonSearch):Observable<Vendor[]>{
    return this._http.post<Vendor[]>(this.API_URL,searchModel)
  }
  getManagerLevels(id:number):Observable<ManagerLevel[]>{
    return this._http.get<ManagerLevel[]>(`${this.API_URL}/GetManagerLevels?id=${id}`)
     }
 

}
