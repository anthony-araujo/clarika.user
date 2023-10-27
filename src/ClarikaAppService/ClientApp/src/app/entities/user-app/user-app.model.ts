import dayjs from "dayjs/esm";
import { IUserLocation } from "app/entities/user-location/user-location.model";
import { ICountry } from "app/entities/country/country.model";

export interface IUserApp {
  id?: number;
  firstName?: string;
  lastName?: string;
  email?: string | null;
  dateBirth?: dayjs.Dayjs | null;
  age?: number | null;
  passwordHash?: string | null;
  securityStamp?: string | null;
  concurrencyStamp?: string | null;
  userLocations?: IUserLocation[] | null;
  country?: ICountry | null;
}

export class UserApp implements IUserApp {
  constructor(
    public id?: number,
    public firstName?: string,
    public lastName?: string,
    public email?: string | null,
    public dateBirth?: dayjs.Dayjs | null,
    public age?: number | null,
    public passwordHash?: string | null,
    public securityStamp?: string | null,
    public concurrencyStamp?: string | null,
    public userLocations?: IUserLocation[] | null,
    public country?: ICountry | null
  ) {}
}

export function getUserAppIdentifier(userApp: IUserApp): number | undefined {
  return userApp.id;
}
