import { ApiAddress } from '../Models/common-models';
import { MAPS_ADDRESS_TYPES, RoleEnum, SiteRolesId } from './constants';

export const csvMaker = (data: any[], csvTitle: string) => {
  const headers = Object.keys(data[0]);
  const values = data.map((item) =>
    headers.map((header) => {
      const value = item[header];
      return typeof value === 'string' && value.includes(',')
        ? `"${value}"`
        : value;
    })
  );
  const csvData = [
    headers.join(','),
    ...values.map((value) => value.join(',')),
  ].join('\n');
  downloadCsvFile(csvData, csvTitle);
};

export const downloadCsvFile = (data: any, csvTitle: string) => {
  const blobData = new Blob([data], { type: 'text/csv' });
  const url = URL.createObjectURL(blobData);
  const a = document.createElement('a');
  (a.href = url), (a.download = csvTitle);
  a.click();
};

export function parseJwt(token: string) {
  try {
    const base64HeaderUrl = token.split('.')[0];
    const base64Header = base64HeaderUrl.replace('-', '+').replace('_', '/');
    const headerData = JSON.parse(window.atob(base64Header));
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace('-', '+').replace('_', '/');
    const dataJWT = JSON.parse(window.atob(base64));
    dataJWT.header = headerData;

    return dataJWT;
  } catch (err) {
    return false;
  }
}

export function getRoleIdByRoleName(roleName: string): number {
  switch (roleName) {
    case RoleEnum.GearedAdmin:
      return SiteRolesId.GearedAdmin;
    case RoleEnum.GearedSalesRep:
      return SiteRolesId.GearedSalesRep;
    case RoleEnum.GearedSuperAdmin:
      return SiteRolesId.GearedSuperAdmin;
    case RoleEnum.VendorGuestUser:
      return SiteRolesId.VendorGuestUser;
    case RoleEnum.VendorManager:
      return SiteRolesId.VendorManager;
    case RoleEnum.VendorSalesRep:
      return SiteRolesId.VendorSalesRep;
  }
  return -1;
}

export function getAddressFromApi(
  place: google.maps.places.PlaceResult
): ApiAddress {
  let address = '';
  let postcode = '';
  let state = '';
  let suburb = '';
  let streetNumber = '';
  let route = '';
  if (place && place.address_components) {
    place.address_components.forEach((component) => {
      const componentType = component.types[0];

      switch (componentType) {
        case MAPS_ADDRESS_TYPES.STREET_NUMBER:
          streetNumber = component.short_name;
          break;
        case MAPS_ADDRESS_TYPES.ROUTE:
          route = component.short_name;
          break;
        case MAPS_ADDRESS_TYPES.POSTAL_CODE:
          postcode = component.short_name;
          break;
        case MAPS_ADDRESS_TYPES.LOCALITY:
          suburb = component.short_name;
          break;
        case MAPS_ADDRESS_TYPES.ADMINISTRATIVE_AREA_LEVEL_1:
          state = component.short_name;
          break;
      }
    });

    if (streetNumber || route) {
      address = `${streetNumber} ${route}`.trim();
    }

    if (!address) {
      address = `${suburb} ${state} ${postcode}`.trim();
    }
  }
  return { address, postcode, state, suburb, streetNumber, route };
}
