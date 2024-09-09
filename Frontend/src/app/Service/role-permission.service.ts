import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import {
  ModuleType,
  RoleModel,
  RolePermissionDTO,
} from '../Models/role-management.model';
import { CommonSearch } from '../Models/common-search.model';

@Injectable({
  providedIn: 'root',
})
export class RolePermissionService {
  private API_URL = `${environment.BASE_URL}/RolePermision`;
  constructor(private _http: HttpClient) {}

  getModules(): Observable<ModuleType[]> {
    return this._http.get<ModuleType[]>(this.API_URL);
  }
  getRigthsForEdit(roleId: number): Observable<RolePermissionDTO[]> {
    return this._http.get<RolePermissionDTO[]>(
      `${this.API_URL}/Rights?roleId=${roleId}`
    );
  }
  getRoles(searchingModel: CommonSearch): Observable<RoleModel[]> {
    return this._http.post<RoleModel[]>(
      `${this.API_URL}/Roles`,
      searchingModel
    );
  }

  upsertRolePermissions(rights: RolePermissionDTO[]) {
    return this._http.post(this.API_URL, rights);
  }
}
