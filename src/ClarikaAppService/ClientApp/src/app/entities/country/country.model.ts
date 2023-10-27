import { IState } from "app/entities/state/state.model";
import { IUserLocation } from "app/entities/user-location/user-location.model";
import { IUserApp } from "app/entities/user-app/user-app.model";

export interface ICountry {
  id?: number;
  name?: string;
  states?: IState[] | null;
  userLocations?: IUserLocation[] | null;
  userApps?: IUserApp[] | null;
}

export class Country implements ICountry {
  constructor(
    public id?: number,
    public name?: string,
    public states?: IState[] | null,
    public userLocations?: IUserLocation[] | null,
    public userApps?: IUserApp[] | null
  ) {}
}

export function getCountryIdentifier(country: ICountry): number | undefined {
  return country.id;
}
