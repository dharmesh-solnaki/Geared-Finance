import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import {
  BaseResponse,
  HeaderSearchModel,
  UserStatus,
  Vendor,
} from '../Models/common-models';
import { LeadsDisplay } from '../Models/lead-display.model';
import { FilterParams } from '../Models/common-filter-params.model';
import { CommonSearch } from '../Models/common-search.model';

@Injectable({
  providedIn: 'root',
})
export class ApplicationService {
  private API_URL = `${environment.BASE_URL}/Application`;
  constructor(private _http: HttpClient) {}

  getOwnerList(): Observable<HeaderSearchModel[]> {
    return this._http.get<HeaderSearchModel[]>(`${this.API_URL}/SalesRep`);
  }
  getLeadList(
    searchParams: FilterParams,
    searchModel: CommonSearch
  ): Observable<BaseResponse<LeadsDisplay>> {
    const leadPayload = {
      searchParams: searchParams,
      searchModel: searchModel,
    };

    return this._http.post<BaseResponse<LeadsDisplay>>(
      `${this.API_URL}/Leads`,
      leadPayload
    );
  }
  getVendorSalesRep(vendorId: number): Observable<Vendor[]> {
    return this._http.get<Vendor[]>(`${this.API_URL}/VendorRep?id=${vendorId}`);
  }

  getUserStatus():Observable<UserStatus> {
    return this._http.get<UserStatus>(`${this.API_URL}/GetUserStatus`);
  }
}
