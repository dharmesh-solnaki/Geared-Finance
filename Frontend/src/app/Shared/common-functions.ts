import { RoleEnum, SiteRolesId } from './constants';

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
    // Get Token Header
    const base64HeaderUrl = token.split('.')[0];
    const base64Header = base64HeaderUrl.replace('-', '+').replace('_', '/');
    const headerData = JSON.parse(window.atob(base64Header));

    // Get Token payload and date's
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace('-', '+').replace('_', '/');
    const dataJWT = JSON.parse(window.atob(base64));
    dataJWT.header = headerData;

    // TODO: add expiration at check ...

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
