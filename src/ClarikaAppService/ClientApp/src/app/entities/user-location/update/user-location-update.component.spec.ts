import { ComponentFixture, TestBed } from "@angular/core/testing";
import { HttpResponse } from "@angular/common/http";
import { HttpClientTestingModule } from "@angular/common/http/testing";
import { FormBuilder } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { RouterTestingModule } from "@angular/router/testing";
import { of, Subject, from } from "rxjs";

import { UserLocationService } from "../service/user-location.service";
import { IUserLocation, UserLocation } from "../user-location.model";
import { ICountry } from "app/entities/country/country.model";
import { CountryService } from "app/entities/country/service/country.service";
import { ILocationType } from "app/entities/location-type/location-type.model";
import { LocationTypeService } from "app/entities/location-type/service/location-type.service";
import { IUserApp } from "app/entities/user-app/user-app.model";
import { UserAppService } from "app/entities/user-app/service/user-app.service";

import { UserLocationUpdateComponent } from "./user-location-update.component";

describe("UserLocation Management Update Component", () => {
  let comp: UserLocationUpdateComponent;
  let fixture: ComponentFixture<UserLocationUpdateComponent>;
  let activatedRoute: ActivatedRoute;
  let userLocationService: UserLocationService;
  let countryService: CountryService;
  let locationTypeService: LocationTypeService;
  let userAppService: UserAppService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule.withRoutes([])],
      declarations: [UserLocationUpdateComponent],
      providers: [
        FormBuilder,
        {
          provide: ActivatedRoute,
          useValue: {
            params: from([{}]),
          },
        },
      ],
    })
      .overrideTemplate(UserLocationUpdateComponent, "")
      .compileComponents();

    fixture = TestBed.createComponent(UserLocationUpdateComponent);
    activatedRoute = TestBed.inject(ActivatedRoute);
    userLocationService = TestBed.inject(UserLocationService);
    countryService = TestBed.inject(CountryService);
    locationTypeService = TestBed.inject(LocationTypeService);
    userAppService = TestBed.inject(UserAppService);

    comp = fixture.componentInstance;
  });

  describe("ngOnInit", () => {
    it("Should call Country query and add missing value", () => {
      const userLocation: IUserLocation = { id: 456 };
      const country: ICountry = { id: 98742 };
      userLocation.country = country;

      const countryCollection: ICountry[] = [{ id: 929 }];
      jest
        .spyOn(countryService, "query")
        .mockReturnValue(of(new HttpResponse({ body: countryCollection })));
      const additionalCountries = [country];
      const expectedCollection: ICountry[] = [
        ...additionalCountries,
        ...countryCollection,
      ];
      jest
        .spyOn(countryService, "addCountryToCollectionIfMissing")
        .mockReturnValue(expectedCollection);

      activatedRoute.data = of({ userLocation });
      comp.ngOnInit();

      expect(countryService.query).toHaveBeenCalled();
      expect(
        countryService.addCountryToCollectionIfMissing
      ).toHaveBeenCalledWith(countryCollection, ...additionalCountries);
      expect(comp.countriesSharedCollection).toEqual(expectedCollection);
    });

    it("Should call LocationType query and add missing value", () => {
      const userLocation: IUserLocation = { id: 456 };
      const locationType: ILocationType = { id: 88006 };
      userLocation.locationType = locationType;

      const locationTypeCollection: ILocationType[] = [{ id: 51706 }];
      jest
        .spyOn(locationTypeService, "query")
        .mockReturnValue(
          of(new HttpResponse({ body: locationTypeCollection }))
        );
      const additionalLocationTypes = [locationType];
      const expectedCollection: ILocationType[] = [
        ...additionalLocationTypes,
        ...locationTypeCollection,
      ];
      jest
        .spyOn(locationTypeService, "addLocationTypeToCollectionIfMissing")
        .mockReturnValue(expectedCollection);

      activatedRoute.data = of({ userLocation });
      comp.ngOnInit();

      expect(locationTypeService.query).toHaveBeenCalled();
      expect(
        locationTypeService.addLocationTypeToCollectionIfMissing
      ).toHaveBeenCalledWith(
        locationTypeCollection,
        ...additionalLocationTypes
      );
      expect(comp.locationTypesSharedCollection).toEqual(expectedCollection);
    });

    it("Should call UserApp query and add missing value", () => {
      const userLocation: IUserLocation = { id: 456 };
      const userApp: IUserApp = { id: 5351 };
      userLocation.userApp = userApp;

      const userAppCollection: IUserApp[] = [{ id: 40043 }];
      jest
        .spyOn(userAppService, "query")
        .mockReturnValue(of(new HttpResponse({ body: userAppCollection })));
      const additionalUserApps = [userApp];
      const expectedCollection: IUserApp[] = [
        ...additionalUserApps,
        ...userAppCollection,
      ];
      jest
        .spyOn(userAppService, "addUserAppToCollectionIfMissing")
        .mockReturnValue(expectedCollection);

      activatedRoute.data = of({ userLocation });
      comp.ngOnInit();

      expect(userAppService.query).toHaveBeenCalled();
      expect(
        userAppService.addUserAppToCollectionIfMissing
      ).toHaveBeenCalledWith(userAppCollection, ...additionalUserApps);
      expect(comp.userAppsSharedCollection).toEqual(expectedCollection);
    });

    it("Should update editForm", () => {
      const userLocation: IUserLocation = { id: 456 };
      const country: ICountry = { id: 89711 };
      userLocation.country = country;
      const locationType: ILocationType = { id: 16764 };
      userLocation.locationType = locationType;
      const userApp: IUserApp = { id: 59926 };
      userLocation.userApp = userApp;

      activatedRoute.data = of({ userLocation });
      comp.ngOnInit();

      expect(comp.editForm.value).toEqual(
        expect.objectContaining(userLocation)
      );
      expect(comp.countriesSharedCollection).toContain(country);
      expect(comp.locationTypesSharedCollection).toContain(locationType);
      expect(comp.userAppsSharedCollection).toContain(userApp);
    });
  });

  describe("save", () => {
    it("Should call update service on save for existing entity", () => {
      // GIVEN
      const saveSubject = new Subject<HttpResponse<UserLocation>>();
      const userLocation = { id: 123 };
      jest.spyOn(userLocationService, "update").mockReturnValue(saveSubject);
      jest.spyOn(comp, "previousState");
      activatedRoute.data = of({ userLocation });
      comp.ngOnInit();

      // WHEN
      comp.save();
      expect(comp.isSaving).toEqual(true);
      saveSubject.next(new HttpResponse({ body: userLocation }));
      saveSubject.complete();

      // THEN
      expect(comp.previousState).toHaveBeenCalled();
      expect(userLocationService.update).toHaveBeenCalledWith(userLocation);
      expect(comp.isSaving).toEqual(false);
    });

    it("Should call create service on save for new entity", () => {
      // GIVEN
      const saveSubject = new Subject<HttpResponse<UserLocation>>();
      const userLocation = new UserLocation();
      jest.spyOn(userLocationService, "create").mockReturnValue(saveSubject);
      jest.spyOn(comp, "previousState");
      activatedRoute.data = of({ userLocation });
      comp.ngOnInit();

      // WHEN
      comp.save();
      expect(comp.isSaving).toEqual(true);
      saveSubject.next(new HttpResponse({ body: userLocation }));
      saveSubject.complete();

      // THEN
      expect(userLocationService.create).toHaveBeenCalledWith(userLocation);
      expect(comp.isSaving).toEqual(false);
      expect(comp.previousState).toHaveBeenCalled();
    });

    it("Should set isSaving to false on error", () => {
      // GIVEN
      const saveSubject = new Subject<HttpResponse<UserLocation>>();
      const userLocation = { id: 123 };
      jest.spyOn(userLocationService, "update").mockReturnValue(saveSubject);
      jest.spyOn(comp, "previousState");
      activatedRoute.data = of({ userLocation });
      comp.ngOnInit();

      // WHEN
      comp.save();
      expect(comp.isSaving).toEqual(true);
      saveSubject.error("This is an error!");

      // THEN
      expect(userLocationService.update).toHaveBeenCalledWith(userLocation);
      expect(comp.isSaving).toEqual(false);
      expect(comp.previousState).not.toHaveBeenCalled();
    });
  });

  describe("Tracking relationships identifiers", () => {
    describe("trackCountryById", () => {
      it("Should return tracked Country primary key", () => {
        const entity = { id: 123 };
        const trackResult = comp.trackCountryById(0, entity);
        expect(trackResult).toEqual(entity.id);
      });
    });

    describe("trackLocationTypeById", () => {
      it("Should return tracked LocationType primary key", () => {
        const entity = { id: 123 };
        const trackResult = comp.trackLocationTypeById(0, entity);
        expect(trackResult).toEqual(entity.id);
      });
    });

    describe("trackUserAppById", () => {
      it("Should return tracked UserApp primary key", () => {
        const entity = { id: 123 };
        const trackResult = comp.trackUserAppById(0, entity);
        expect(trackResult).toEqual(entity.id);
      });
    });
  });
});
