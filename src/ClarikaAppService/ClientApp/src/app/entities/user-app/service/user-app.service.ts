import { Injectable } from "@angular/core";
import { HttpClient, HttpResponse } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import dayjs from "dayjs/esm";

import { isPresent } from "app/core/util/operators";
import { DATE_FORMAT } from "app/config/input.constants";
import { ApplicationConfigService } from "app/core/config/application-config.service";
import { createRequestOption } from "app/core/request/request-util";
import { IUserApp, getUserAppIdentifier } from "../user-app.model";

export type EntityResponseType = HttpResponse<IUserApp>;
export type EntityArrayResponseType = HttpResponse<IUserApp[]>;

@Injectable({ providedIn: "root" })
export class UserAppService {
  protected resourceUrl =
    this.applicationConfigService.getEndpointFor("api/user-apps");

  constructor(
    protected http: HttpClient,
    protected applicationConfigService: ApplicationConfigService
  ) {}

  create(userApp: IUserApp): Observable<EntityResponseType> {
    const copy = this.convertDateFromClient(userApp);
    return this.http
      .post<IUserApp>(this.resourceUrl, copy, { observe: "response" })
      .pipe(map((res: EntityResponseType) => this.convertDateFromServer(res)));
  }

  update(userApp: IUserApp): Observable<EntityResponseType> {
    const copy = this.convertDateFromClient(userApp);
    return this.http
      .put<IUserApp>(
        `${this.resourceUrl}/${getUserAppIdentifier(userApp) as number}`,
        copy,
        { observe: "response" }
      )
      .pipe(map((res: EntityResponseType) => this.convertDateFromServer(res)));
  }

  partialUpdate(userApp: IUserApp): Observable<EntityResponseType> {
    const copy = this.convertDateFromClient(userApp);
    return this.http
      .patch<IUserApp>(
        `${this.resourceUrl}/${getUserAppIdentifier(userApp) as number}`,
        copy,
        { observe: "response" }
      )
      .pipe(map((res: EntityResponseType) => this.convertDateFromServer(res)));
  }

  find(id: number): Observable<EntityResponseType> {
    return this.http
      .get<IUserApp>(`${this.resourceUrl}/${id}`, { observe: "response" })
      .pipe(map((res: EntityResponseType) => this.convertDateFromServer(res)));
  }

  query(req?: any): Observable<EntityArrayResponseType> {
    const options = createRequestOption(req);
    return this.http
      .get<IUserApp[]>(this.resourceUrl, {
        params: options,
        observe: "response",
      })
      .pipe(
        map((res: EntityArrayResponseType) =>
          this.convertDateArrayFromServer(res)
        )
      );
  }

  delete(id: number): Observable<HttpResponse<{}>> {
    return this.http.delete(`${this.resourceUrl}/${id}`, {
      observe: "response",
    });
  }

  addUserAppToCollectionIfMissing(
    userAppCollection: IUserApp[],
    ...userAppsToCheck: (IUserApp | null | undefined)[]
  ): IUserApp[] {
    const userApps: IUserApp[] = userAppsToCheck.filter(isPresent);
    if (userApps.length > 0) {
      const userAppCollectionIdentifiers = userAppCollection.map(
        (userAppItem) => getUserAppIdentifier(userAppItem)!
      );
      const userAppsToAdd = userApps.filter((userAppItem) => {
        const userAppIdentifier = getUserAppIdentifier(userAppItem);
        if (
          userAppIdentifier == null ||
          userAppCollectionIdentifiers.includes(userAppIdentifier)
        ) {
          return false;
        }
        userAppCollectionIdentifiers.push(userAppIdentifier);
        return true;
      });
      return [...userAppsToAdd, ...userAppCollection];
    }
    return userAppCollection;
  }

  protected convertDateFromClient(userApp: IUserApp): IUserApp {
    return Object.assign({}, userApp, {
      dateBirth: userApp.dateBirth?.isValid()
        ? userApp.dateBirth.format(DATE_FORMAT)
        : undefined,
    });
  }

  protected convertDateFromServer(res: EntityResponseType): EntityResponseType {
    if (res.body) {
      res.body.dateBirth = res.body.dateBirth
        ? dayjs(res.body.dateBirth)
        : undefined;
    }
    return res;
  }

  protected convertDateArrayFromServer(
    res: EntityArrayResponseType
  ): EntityArrayResponseType {
    if (res.body) {
      res.body.forEach((userApp: IUserApp) => {
        userApp.dateBirth = userApp.dateBirth
          ? dayjs(userApp.dateBirth)
          : undefined;
      });
    }
    return res;
  }
}
