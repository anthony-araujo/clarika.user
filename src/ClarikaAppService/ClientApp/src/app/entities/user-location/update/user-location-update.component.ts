import { Component, OnInit } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { FormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { Observable } from "rxjs";
import { finalize, map } from "rxjs/operators";

import { IUserLocation, UserLocation } from "../user-location.model";
import { UserLocationService } from "../service/user-location.service";
import { ICountry } from "app/entities/country/country.model";
import { CountryService } from "app/entities/country/service/country.service";
import { ILocationType } from "app/entities/location-type/location-type.model";
import { LocationTypeService } from "app/entities/location-type/service/location-type.service";
import { IUserApp } from "app/entities/user-app/user-app.model";
import { UserAppService } from "app/entities/user-app/service/user-app.service";

@Component({
  selector: "jhi-user-location-update",
  templateUrl: "./user-location-update.component.html",
})
export class UserLocationUpdateComponent implements OnInit {
  isSaving = false;

  countriesSharedCollection: ICountry[] = [];
  locationTypesSharedCollection: ILocationType[] = [];
  userAppsSharedCollection: IUserApp[] = [];

  editForm = this.fb.group({
    id: [],
    address: [null, [Validators.required]],
    zipCode: [],
    province: [],
    country: [],
    locationType: [],
    userApp: [],
  });

  constructor(
    protected userLocationService: UserLocationService,
    protected countryService: CountryService,
    protected locationTypeService: LocationTypeService,
    protected userAppService: UserAppService,
    protected activatedRoute: ActivatedRoute,
    protected fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(({ userLocation }) => {
      this.updateForm(userLocation);

      this.loadRelationshipsOptions();
    });
  }

  previousState(): void {
    window.history.back();
  }

  save(): void {
    this.isSaving = true;
    const userLocation = this.createFromForm();
    if (userLocation.id !== undefined) {
      this.subscribeToSaveResponse(
        this.userLocationService.update(userLocation)
      );
    } else {
      this.subscribeToSaveResponse(
        this.userLocationService.create(userLocation)
      );
    }
  }

  trackCountryById(_index: number, item: ICountry): number {
    return item.id!;
  }

  trackLocationTypeById(_index: number, item: ILocationType): number {
    return item.id!;
  }

  trackUserAppById(_index: number, item: IUserApp): number {
    return item.id!;
  }

  protected subscribeToSaveResponse(
    result: Observable<HttpResponse<IUserLocation>>
  ): void {
    result.pipe(finalize(() => this.onSaveFinalize())).subscribe({
      next: () => this.onSaveSuccess(),
      error: () => this.onSaveError(),
    });
  }

  protected onSaveSuccess(): void {
    this.previousState();
  }

  protected onSaveError(): void {
    // Api for inheritance.
  }

  protected onSaveFinalize(): void {
    this.isSaving = false;
  }

  protected updateForm(userLocation: IUserLocation): void {
    this.editForm.patchValue({
      id: userLocation.id,
      address: userLocation.address,
      zipCode: userLocation.zipCode,
      province: userLocation.province,
      country: userLocation.country,
      locationType: userLocation.locationType,
      userApp: userLocation.userApp,
    });

    this.countriesSharedCollection =
      this.countryService.addCountryToCollectionIfMissing(
        this.countriesSharedCollection,
        userLocation.country
      );
    this.locationTypesSharedCollection =
      this.locationTypeService.addLocationTypeToCollectionIfMissing(
        this.locationTypesSharedCollection,
        userLocation.locationType
      );
    this.userAppsSharedCollection =
      this.userAppService.addUserAppToCollectionIfMissing(
        this.userAppsSharedCollection,
        userLocation.userApp
      );
  }

  protected loadRelationshipsOptions(): void {
    this.countryService
      .query()
      .pipe(map((res: HttpResponse<ICountry[]>) => res.body ?? []))
      .pipe(
        map((countries: ICountry[]) =>
          this.countryService.addCountryToCollectionIfMissing(
            countries,
            this.editForm.get("country")!.value
          )
        )
      )
      .subscribe(
        (countries: ICountry[]) => (this.countriesSharedCollection = countries)
      );

    this.locationTypeService
      .query()
      .pipe(map((res: HttpResponse<ILocationType[]>) => res.body ?? []))
      .pipe(
        map((locationTypes: ILocationType[]) =>
          this.locationTypeService.addLocationTypeToCollectionIfMissing(
            locationTypes,
            this.editForm.get("locationType")!.value
          )
        )
      )
      .subscribe(
        (locationTypes: ILocationType[]) =>
          (this.locationTypesSharedCollection = locationTypes)
      );

    this.userAppService
      .query()
      .pipe(map((res: HttpResponse<IUserApp[]>) => res.body ?? []))
      .pipe(
        map((userApps: IUserApp[]) =>
          this.userAppService.addUserAppToCollectionIfMissing(
            userApps,
            this.editForm.get("userApp")!.value
          )
        )
      )
      .subscribe(
        (userApps: IUserApp[]) => (this.userAppsSharedCollection = userApps)
      );
  }

  protected createFromForm(): IUserLocation {
    return {
      ...new UserLocation(),
      id: this.editForm.get(["id"])!.value,
      address: this.editForm.get(["address"])!.value,
      zipCode: this.editForm.get(["zipCode"])!.value,
      province: this.editForm.get(["province"])!.value,
      country: this.editForm.get(["country"])!.value,
      locationType: this.editForm.get(["locationType"])!.value,
      userApp: this.editForm.get(["userApp"])!.value,
    };
  }
}
