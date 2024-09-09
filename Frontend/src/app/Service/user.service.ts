import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../Models/user.model';
import { environment } from 'src/environments/environment.development';
import { CommonSearch } from 'src/app/Models/common-search.model';
import { BaseResponse, IsExistData } from '../Models/common-models';
import { RelationshipManager } from '../Models/RelationshipManager.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private API_URL: string = `${environment.BASE_URL}/user`;

  constructor(private _http: HttpClient) {}

  getUsers(searchingModel: CommonSearch): Observable<BaseResponse<User>> {
    return this._http.post<BaseResponse<User>>(
      `${this.API_URL}/Users`,
      searchingModel
    );
  }
  addUser(user: User): Observable<IsExistData> {
    return this._http.post<IsExistData>(`${this.API_URL}/User`, user);
  }

  getRelationshipManagers(): Observable<RelationshipManager[]> {
    return this._http.get<RelationshipManager[]>(
      `${this.API_URL}/RelationShipManagers`
    );
  }

  getReportingTo(
    vendorId: number,
    managerLevelId: number
  ): Observable<RelationshipManager[]> {
    return this._http.get<RelationshipManager[]>(
      `${this.API_URL}/ReportingTo?vendorId=${vendorId}&managerLevelId=${managerLevelId}`
    );
  }
  deleteUser(id: number) {
    return this._http.delete(`${this.API_URL}?id=${id}`);
  }
}
