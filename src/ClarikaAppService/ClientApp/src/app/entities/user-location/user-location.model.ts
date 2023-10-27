import { ICountry } from "app/entities/country/country.model";
import { ILocationType } from "app/entities/location-type/location-type.model";
import { IUserApp } from "app/entities/user-app/user-app.model";

export interface IUserLocation {
  id?: number;
  address?: string;
  zipCode?: string | null;
  province?: string | null;
  country?: ICountry | null;
  locationType?: ILocationType | null;
  userApp?: IUserApp | null;
}

export class UserLocation implements IUserLocation {
  constructor(
    public id?: number,
    public address?: string,
    public zipCode?: string | null,
    public province?: string | null,
    public country?: ICountry | null,
    public locationType?: ILocationType | null,
    public userApp?: IUserApp | null
  ) {}
}

export function getUserLocationIdentifier(
  userLocation: IUserLocation
): number | undefined {
  return userLocation.id;
}
