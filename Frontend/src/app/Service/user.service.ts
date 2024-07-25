import { Injectable } from '@angular/core';
import { HttpClient,  } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../Models/user.model';
import { environment } from 'src/environments/environment.development';
import { CommonSearch } from 'src/app/Models/common-search.model';
import {IsExistData,  } from '../Models/common-models';
import { RelationshipManager } from '../Models/RelationshipManager.model';


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
 
  updateUser(user:User):Observable<User>{
   return this._http.put<User>(this.API_URL,user)
  }
  getRelationshipManagers():Observable<RelationshipManager[]>{
 return this._http.get<RelationshipManager[]>(`${this.API_URL}/GetRelationShipManager`)
  }
 
  getReportingTo(vendorId:number,managerLevelId:number):Observable<RelationshipManager[]>{

    return this._http.get<RelationshipManager[]>(`${this.API_URL}/GetReportingTo?vendorId=${vendorId}&managerLevelId=${managerLevelId}`)
  }
  deleteUser(id:number){
   return this._http.delete( `${this.API_URL}?${id}`)
  }
  checkValidityofEmailAndPassword(email:string,mobile:string):Observable<IsExistData>{
    return this._http.get<IsExistData>(`${this.API_URL}/CheckValidity?email=${email}&mobile=${mobile}`);
  }
 
}
