import { Injectable } from "@angular/core";
import { HttpClient, HttpResponse } from "@angular/common/http";
import { Observable } from "rxjs";

import { isPresent } from "app/core/util/operators";
import { ApplicationConfigService } from "app/core/config/application-config.service";
import { createRequestOption } from "app/core/request/request-util";
import {
  ILocationType,
  getLocationTypeIdentifier,
} from "../location-type.model";

export type EntityResponseType = HttpResponse<ILocationType>;
export type EntityArrayResponseType = HttpResponse<ILocationType[]>;

@Injectable({ providedIn: "root" })
export class LocationTypeService {
  protected resourceUrl =
    this.applicationConfigService.getEndpointFor("api/location-types");

  constructor(
    protected http: HttpClient,
    protected applicationConfigService: ApplicationConfigService
  ) {}

  create(locationType: ILocationType): Observable<EntityResponseType> {
    return this.http.post<ILocationType>(this.resourceUrl, locationType, {
      observe: "response",
    });
  }

  update(locationType: ILocationType): Observable<EntityResponseType> {
    return this.http.put<ILocationType>(
      `${this.resourceUrl}/${
        getLocationTypeIdentifier(locationType) as number
      }`,
      locationType,
      { observe: "response" }
    );
  }

  partialUpdate(locationType: ILocationType): Observable<EntityResponseType> {
    return this.http.patch<ILocationType>(
      `${this.resourceUrl}/${
        getLocationTypeIdentifier(locationType) as number
      }`,
      locationType,
      { observe: "response" }
    );
  }

  find(id: number): Observable<EntityResponseType> {
    return this.http.get<ILocationType>(`${this.resourceUrl}/${id}`, {
      observe: "response",
    });
  }

  query(req?: any): Observable<EntityArrayResponseType> {
    const options = createRequestOption(req);
    return this.http.get<ILocationType[]>(this.resourceUrl, {
      params: options,
      observe: "response",
    });
  }

  delete(id: number): Observable<HttpResponse<{}>> {
    return this.http.delete(`${this.resourceUrl}/${id}`, {
      observe: "response",
    });
  }

  addLocationTypeToCollectionIfMissing(
    locationTypeCollection: ILocationType[],
    ...locationTypesToCheck: (ILocationType | null | undefined)[]
  ): ILocationType[] {
    const locationTypes: ILocationType[] =
      locationTypesToCheck.filter(isPresent);
    if (locationTypes.length > 0) {
      const locationTypeCollectionIdentifiers = locationTypeCollection.map(
        (locationTypeItem) => getLocationTypeIdentifier(locationTypeItem)!
      );
      const locationTypesToAdd = locationTypes.filter((locationTypeItem) => {
        const locationTypeIdentifier =
          getLocationTypeIdentifier(locationTypeItem);
        if (
          locationTypeIdentifier == null ||
          locationTypeCollectionIdentifiers.includes(locationTypeIdentifier)
        ) {
          return false;
        }
        locationTypeCollectionIdentifiers.push(locationTypeIdentifier);
        return true;
      });
      return [...locationTypesToAdd, ...locationTypeCollection];
    }
    return locationTypeCollection;
  }
}
