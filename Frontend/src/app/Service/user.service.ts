import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../Models/user.model';
import { environment } from 'src/environments/environment.development';
import { CommonSearch } from 'src/app/Models/common-search.model';
import { Vendor } from '../Models/common-models';

@Injectable({
  providedIn: 'root'
})
export class UserService {
private API_URL:string = `${environment.BASE_URL}/user`;

  constructor(private _http:HttpClient ) {}

  getUsers(searchingModel:CommonSearch):Observable<User[]>{
   
    return this._http.post<User[]>(`${this.API_URL}/GetUsers`,searchingModel);
  }
  addUser(user:User): Observable<User>{    
    
    return this._http.post<User>(`${this.API_URL}/AddUser`,user);
  }
  getVendors():Observable<Vendor[]>{
    return this._http.get<Vendor[]>(this.API_URL)
  }
  // updateUser(id:number,user:User){
  //  return this._http.put<User>(this.API_URL/{id})
  // }
}
