import { ComponentFixture, TestBed } from "@angular/core/testing";
import { HttpResponse } from "@angular/common/http";
import { HttpClientTestingModule } from "@angular/common/http/testing";
import { FormBuilder } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { RouterTestingModule } from "@angular/router/testing";
import { of, Subject, from } from "rxjs";

import { CityService } from "../service/city.service";
import { ICity, City } from "../city.model";
import { IState } from "app/entities/state/state.model";
import { StateService } from "app/entities/state/service/state.service";

import { CityUpdateComponent } from "./city-update.component";

describe("City Management Update Component", () => {
  let comp: CityUpdateComponent;
  let fixture: ComponentFixture<CityUpdateComponent>;
  let activatedRoute: ActivatedRoute;
  let cityService: CityService;
  let stateService: StateService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule.withRoutes([])],
      declarations: [CityUpdateComponent],
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
      .overrideTemplate(CityUpdateComponent, "")
      .compileComponents();

    fixture = TestBed.createComponent(CityUpdateComponent);
    activatedRoute = TestBed.inject(ActivatedRoute);
    cityService = TestBed.inject(CityService);
    stateService = TestBed.inject(StateService);

    comp = fixture.componentInstance;
  });

  describe("ngOnInit", () => {
    it("Should call State query and add missing value", () => {
      const city: ICity = { id: 456 };
      const state: IState = { id: 18297 };
      city.state = state;

      const stateCollection: IState[] = [{ id: 60814 }];
      jest
        .spyOn(stateService, "query")
        .mockReturnValue(of(new HttpResponse({ body: stateCollection })));
      const additionalStates = [state];
      const expectedCollection: IState[] = [
        ...additionalStates,
        ...stateCollection,
      ];
      jest
        .spyOn(stateService, "addStateToCollectionIfMissing")
        .mockReturnValue(expectedCollection);

      activatedRoute.data = of({ city });
      comp.ngOnInit();

      expect(stateService.query).toHaveBeenCalled();
      expect(stateService.addStateToCollectionIfMissing).toHaveBeenCalledWith(
        stateCollection,
        ...additionalStates
      );
      expect(comp.statesSharedCollection).toEqual(expectedCollection);
    });

    it("Should update editForm", () => {
      const city: ICity = { id: 456 };
      const state: IState = { id: 69338 };
      city.state = state;

      activatedRoute.data = of({ city });
      comp.ngOnInit();

      expect(comp.editForm.value).toEqual(expect.objectContaining(city));
      expect(comp.statesSharedCollection).toContain(state);
    });
  });

  describe("save", () => {
    it("Should call update service on save for existing entity", () => {
      // GIVEN
      const saveSubject = new Subject<HttpResponse<City>>();
      const city = { id: 123 };
      jest.spyOn(cityService, "update").mockReturnValue(saveSubject);
      jest.spyOn(comp, "previousState");
      activatedRoute.data = of({ city });
      comp.ngOnInit();

      // WHEN
      comp.save();
      expect(comp.isSaving).toEqual(true);
      saveSubject.next(new HttpResponse({ body: city }));
      saveSubject.complete();

      // THEN
      expect(comp.previousState).toHaveBeenCalled();
      expect(cityService.update).toHaveBeenCalledWith(city);
      expect(comp.isSaving).toEqual(false);
    });

    it("Should call create service on save for new entity", () => {
      // GIVEN
      const saveSubject = new Subject<HttpResponse<City>>();
      const city = new City();
      jest.spyOn(cityService, "create").mockReturnValue(saveSubject);
      jest.spyOn(comp, "previousState");
      activatedRoute.data = of({ city });
      comp.ngOnInit();

      // WHEN
      comp.save();
      expect(comp.isSaving).toEqual(true);
      saveSubject.next(new HttpResponse({ body: city }));
      saveSubject.complete();

      // THEN
      expect(cityService.create).toHaveBeenCalledWith(city);
      expect(comp.isSaving).toEqual(false);
      expect(comp.previousState).toHaveBeenCalled();
    });

    it("Should set isSaving to false on error", () => {
      // GIVEN
      const saveSubject = new Subject<HttpResponse<City>>();
      const city = { id: 123 };
      jest.spyOn(cityService, "update").mockReturnValue(saveSubject);
      jest.spyOn(comp, "previousState");
      activatedRoute.data = of({ city });
      comp.ngOnInit();

      // WHEN
      comp.save();
      expect(comp.isSaving).toEqual(true);
      saveSubject.error("This is an error!");

      // THEN
      expect(cityService.update).toHaveBeenCalledWith(city);
      expect(comp.isSaving).toEqual(false);
      expect(comp.previousState).not.toHaveBeenCalled();
    });
  });

  describe("Tracking relationships identifiers", () => {
    describe("trackStateById", () => {
      it("Should return tracked State primary key", () => {
        const entity = { id: 123 };
        const trackResult = comp.trackStateById(0, entity);
        expect(trackResult).toEqual(entity.id);
      });
    });
  });
});
